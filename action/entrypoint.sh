#!/usr/bin/env bash

server=$1
filename=$2

echo "*** Web Validate ***"
echo "server = $server filename=$filename"

cp /github/workspace/TestFiles/$filename /app/TestFiles/$filename

cd /app

result=$(dotnet /app/webvalidate.dll --server $server --files $filename)
if [[ "$result" == *"Errors"* ]]; then
  echo -e "\e[31mWeb Validate Error:\e[0m"
  echo "$result"
  exit -1
else
  echo "$result"
  exit 0
fi

cd /github/workspace
