#!/bin/bash
export $(grep -v '^#' .env | xargs)
set -x

mkdir out

java -cp ./bin/yaver-generator-cli.jar:./bin/openapi-generator-cli.jar \
  org.openapitools.codegen.OpenAPIGenerator \
  generate \
  -g yaver-cs-fastendpoints \
  -i swagger.yaml \
  -o out/yaver-cs-fastendpoints \
  --additional-properties=packageName="${API_NAME}" \
  --additional-properties=targetFramework="${TARGET_FRAMEWORK}" \
  --additional-properties=packageVersion="${PACKAGE_VERSION}"
