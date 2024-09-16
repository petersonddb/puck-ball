.PHONY: all build

# check: type-check test

# test:
# 	@echo "Running tests..."
# 	pytest -v

# type-check:
# 	@echo "Running type check..."
# 	mypy .

all: vendor build

clean:
	@echo "Removing some generated files..."
	rm -rf modules

vendor:
	@echo "Creating copy of dependencies into the codebase..."
	go mod vendor

build:
	@echo "Building as a shared object (plugin)..."
	go build --trimpath --mod=vendor --buildmode=plugin -o ./backend.so
