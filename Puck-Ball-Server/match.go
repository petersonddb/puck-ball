package main

import (
	"context"
	"database/sql"
	"encoding/json"

	"github.com/heroiclabs/nakama-common/runtime"
)

const PUCK_BALL_MATCH_MODULE_NAME = "puck-ball-match-module"

const (
	STATE_OPCODE_POSITION = 0
)

func CreateTheMatchRPC(
	ctx context.Context,
	logger runtime.Logger,
	db *sql.DB,
	nk runtime.NakamaModule,
	payload string) (string, error) {

	params := map[string]interface{}{}
	matchId, err := nk.MatchCreate(ctx, PUCK_BALL_MATCH_MODULE_NAME, params)

	return matchId, err
}

type PuckBallMatch struct{}

type PuckBallMatchState struct {
	presences  map[string]runtime.Presence
	emptyTicks int
}

type playerPosition struct {
	X float64
	Y float64
	Z float64
}

func (m *PuckBallMatch) MatchInit(
	ctx context.Context,
	logger runtime.Logger,
	db *sql.DB,
	nk runtime.NakamaModule,
	params map[string]interface{}) (interface{}, int, string) {

	state := &PuckBallMatchState{
		emptyTicks: 0,
		presences:  map[string]runtime.Presence{},
	}

	return state, 30, "puck-ball-match"
}

func (m *PuckBallMatch) MatchJoinAttempt(
	ctx context.Context,
	logger runtime.Logger,
	db *sql.DB,
	nk runtime.NakamaModule,
	dispatcher runtime.MatchDispatcher,
	tick int64,
	state interface{},
	presence runtime.Presence,
	metadata map[string]string) (interface{}, bool, string) {

	return state, true, presence.GetSessionId()
}

func (m *PuckBallMatch) MatchJoin(
	ctx context.Context,
	logger runtime.Logger,
	db *sql.DB,
	nk runtime.NakamaModule,
	dispatcher runtime.MatchDispatcher,
	tick int64,
	state interface{},
	presences []runtime.Presence) interface{} {

	puckBallState, ok := state.(*PuckBallMatchState)
	if !ok {
		logger.Warn("There is a problem with the state object!")
		return nil
	}

	for i := 0; i < len(presences); i++ {
		puckBallState.presences[presences[i].GetSessionId()] = presences[i]
	}

	return puckBallState
}

func (m *PuckBallMatch) MatchLeave(ctx context.Context,
	logger runtime.Logger,
	db *sql.DB,
	nk runtime.NakamaModule,
	dispatcher runtime.MatchDispatcher,
	tick int64,
	state interface{},
	presences []runtime.Presence) interface{} {

	puckBallState, ok := state.(*PuckBallMatchState)
	if !ok {
		logger.Warn("There is a problem with the state object!")
		return nil
	}

	for _, leave := range presences {
		delete(puckBallState.presences, leave.GetSessionId())
	}

	return puckBallState
}

func (m *PuckBallMatch) MatchLoop(ctx context.Context,
	logger runtime.Logger,
	db *sql.DB,
	nk runtime.NakamaModule,
	dispatcher runtime.MatchDispatcher,
	tick int64,
	state interface{},
	messages []runtime.MatchData) interface{} {

	puckBallState, ok := state.(*PuckBallMatchState)
	if !ok {
		logger.Error("There is a problem with the state object!")
		return nil
	}

	for _, message := range messages {
		switch message.GetOpCode() {
		case STATE_OPCODE_POSITION:
			var position playerPosition
			if err := json.Unmarshal(message.GetData(), &position); err != nil {
				logger.Error("[MatchLoop] Failed reading POSITION message: %v", err)
			}

			dispatcher.BroadcastMessage(
				STATE_OPCODE_POSITION, message.GetData(), nil, puckBallState.presences[message.GetSessionId()], true)
		}
	}

	return state
}

func (m *PuckBallMatch) MatchTerminate(ctx context.Context,
	logger runtime.Logger,
	db *sql.DB,
	nk runtime.NakamaModule,
	dispatcher runtime.MatchDispatcher,
	tick int64,
	state interface{}, graceSeconds int) interface{} {

	return state
}

func (m *PuckBallMatch) MatchSignal(ctx context.Context,
	logger runtime.Logger,
	db *sql.DB,
	nk runtime.NakamaModule,
	dispatcher runtime.MatchDispatcher,
	tick int64,
	state interface{},
	data string) (interface{}, string) {

	return state, ""
}
