// Adds _id to Revisions, Publications, Deletions, and Categories.
// Adds DateCommitted to Publications.
db = db.getSiblingDB('bhs');
db.posts.updateMany({}, [
  {
    "$addFields": {
      "Revisions": {
        $map: {
          input: "$Revisions",
          as: "revision",
          in: {
            $mergeObjects: [
              "$$revision",
              {
                _id: ObjectId()
              },
              {
                Publications: {
                  $map: {
                    input: "$$revision.Publications",
                    as: "publication",
                    in: {
                      $mergeObjects: [
                        "$$publication",
                        {
                          _id: ObjectId(),
                          DateCommitted: "$$publication.DatePublished"
                        }
                      ]
                    }
                  }
                }
              }
            ]
          }
        }
      },
      "Categories": {
        $map: {
          input: "$Categories",
          as: "category",
          in: {
            $mergeObjects: [
              "$$category",
              {
                Changes: {
                  $map: {
                    input: "$$category.Changes",
                    as: "change",
                    in: {
                      $mergeObjects: [
                        "$$change",
                        {
                          _id: ObjectId()
                        }
                      ]
                    }
                  }
                }
              }
            ]
          }
        }
      },
      "Deletions": {
        $map: {
          input: "$Deletions",
          as: "deletion",
          in: {
            $mergeObjects: [
              "$$deletion",
              {
                _id: ObjectId()
              }
            ]
          }
        }
      }
    }
  }
]);
