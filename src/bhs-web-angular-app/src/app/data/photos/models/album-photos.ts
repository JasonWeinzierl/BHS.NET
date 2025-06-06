import { z } from 'zod/v4';
import { albumSchema } from './album';
import { photoSchema } from './photo';

export const albumPhotosSchema = albumSchema.extend({
  photos: photoSchema.array(),
});

export type AlbumPhotos = z.infer<typeof albumPhotosSchema>;
