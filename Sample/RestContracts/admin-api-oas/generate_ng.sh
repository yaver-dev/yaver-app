#!/bin/bash
export $(grep -v '^#' .env | xargs)
set -x

mkdir out

java -cp ./bin/yaver-generator-cli.jar:./bin/openapi-generator-cli.jar \
  org.openapitools.codegen.OpenAPIGenerator \
  generate \
  -g yaver-ts-angular \
  -i swagger.yaml \
  -o out/ts-angular \
  --additional-properties=npmName="${NPM_NAME}" \
  --additional-properties=configurationPrefix="${MODULE_PREFIX}" \
  --additional-properties=npmVersion="${PACKAGE_VERSION}" \
  --additional-properties=useSingleRequestParameter=true \
  --additional-properties=ngVersion="${NG_VERSION}"
