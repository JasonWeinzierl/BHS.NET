#!/bin/bash
set -euo pipefail

# app-version.ts contains version information about
# a particular build snapshot of this application.
# It should NOT be environment-specific.
# That file is replaced by the output of this script
# by the Angular build process when building the release configuration.

: ${SEMANTIC_VERSION:?"Need to set SEMANTIC_VERSION"}
: ${INFORMATIONAL_VERSION:?"Need to set INFORMATIONAL_VERSION"}
: ${COMMIT_DATE:?"Need to set COMMIT_DATE"}

# Value must match angular.json's `fileReplacements.with`.
filePath="src/environments/app-version.release.ts"

echo "WRITE: $filePath"

echo -n "export default " > $filePath

# Schema must match `app-version.ts`.
jq -n -j \
  --arg semantic_version $SEMANTIC_VERSION \
  --arg informational_version $INFORMATIONAL_VERSION \
  --arg commit_date $COMMIT_DATE \
  '{ "semver": $semantic_version, "info": $informational_version, "commitDate": $commit_date }' \
  >> $filePath

echo ";" >> $filePath

echo "::group::Created File"
cat $filePath
echo "::endgroup::"
