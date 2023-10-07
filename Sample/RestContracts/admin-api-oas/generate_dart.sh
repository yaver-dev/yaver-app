#!/bin/bash
export $(grep -v '^#' .env | xargs)
set -x

mkdir out

java -cp ./bin/yaver-generator-cli.jar:./bin/openapi-generator-cli.jar \
  org.openapitools.codegen.OpenAPIGenerator \
  generate \
  -g dart-dio \
  -i swagger.yaml \
  -o out/dart-dio
