#!/bin/sh

# Considering within Docker
/nakama/nakama migrate up --database.address postgres:localdb@postgres:5432/nakama
/nakama/nakama --database.address postgres:localdb@postgres:5432/nakama --config /nakama/data/nakama.yml
