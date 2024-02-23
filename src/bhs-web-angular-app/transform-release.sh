# app-version.json contains version information about
# a particular build snapshot of this application.
# It should NOT be environment-specific.
# That file is replaced by the output of this script
# by the Angular build process when building the release configuration.
: ${SEMANTIC_VERSION:?"Need to set SEMANTIC_VERSION"}
: ${INFORMATIONAL_VERSION:?"Need to set INFORMATIONAL_VERSION"}
: ${COMMIT_DATE:?"Need to set COMMIT_DATE"}
jq -n \
  --arg semantic_version $SEMANTIC_VERSION \
  --arg informational_version $INFORMATIONAL_VERSION \
  --arg commit_date $COMMIT_DATE \
  '{ "semver": $semantic_version, "info": $informational_version, "commitDate": $commit_date }' \
  > src/environments/app-version.release.json
