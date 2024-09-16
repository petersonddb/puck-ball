package main

import (
	"context"
	"database/sql"

	"github.com/heroiclabs/nakama-common/runtime"
)

func InitModule(
	ctx context.Context,
	logger runtime.Logger,
	db *sql.DB,
	nk runtime.NakamaModule,
	initializer runtime.Initializer) error {

	if err := initializer.RegisterMatch(PUCK_BALL_MATCH_MODULE_NAME, func(ctx context.Context, logger runtime.Logger, db *sql.DB, nk runtime.NakamaModule) (runtime.Match, error) {
		return &PuckBallMatch{}, nil
	}); err != nil {
		logger.Error("Unable to register %v", err)
		return err
	}

	if err := initializer.RegisterRpc("create_the_match_rpc", CreateTheMatchRPC); err != nil {
		logger.Error("Unable to register %v", err)
		return err
	}
	return nil
}
