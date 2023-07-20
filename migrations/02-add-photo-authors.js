// Adds Author to Album and Photo and BannerPhoto.
db = db.getSiblingDB('bhs');

const albumsCursor = db.albums.find({ "AuthorUsername": { $exists: true } });

albumsCursor.forEach(album => {
  const authorUsername = album.AuthorUsername;

  const author = db.authors.findOne({ "_id": authorUsername });

  if (author) {
    db.albums.updateOne(
      { "_id": album._id },
      { $set: { "Author": author } }
    );

    if (album.BannerPhoto && album.BannerPhoto.AuthorUsername === authorUsername) {
      db.albums.updateOne(
        { "_id": album._id },
        { $set: { "BannerPhoto.Author": author } }
      );
    }

    db.albums.updateMany(
      { "_id": album._id, "Photos.AuthorUsername": authorUsername },
      { $set: { "Photos.$[].Author": author } }
    );
  }
});
