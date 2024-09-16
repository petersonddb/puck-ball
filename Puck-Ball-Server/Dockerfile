FROM heroiclabs/nakama-pluginbuilder:3.23.0 AS builder

ENV GO111MODULE on
ENV CGO_ENABLED 1

WORKDIR /backend
COPY . .

RUN make build

FROM heroiclabs/nakama:3.23.0

COPY --from=builder /backend/nakama.yml /nakama/data/
COPY --from=builder /backend/start.sh /nakama/data/

COPY --from=builder /backend/backend.so /nakama/data/modules
COPY --from=builder /backend/*.json /nakama/data/modules

ENTRYPOINT [ "/nakama/data/start.sh" ]
