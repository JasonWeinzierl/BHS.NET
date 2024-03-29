﻿namespace BHS.Contracts.Blog;

public record PostPreview(
    string Slug,
    string Title,
    string ContentPreview,
    Author? Author,
    DateTimeOffset DatePublished,
    IEnumerable<Category> Categories);
