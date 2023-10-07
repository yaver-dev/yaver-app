#!/bin/bash

export $(grep -v '^#' .env | xargs)
set -x

mkdir bin
curl -L https://github.com/yaver-dev/oas-generator/releases/download/${YAVER_VERSION}/codegen.cli.zip -o ./bin/yaver.zip &&
  unzip -j ./bin/yaver.zip -d ./bin
