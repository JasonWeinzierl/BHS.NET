import { z } from 'zod';
import { albumSchema } from './album';
import { photoSchema } from './photo';

export const albumPhotosSchema = albumSchema.extend({
  photos: photoSchema.array(),
});

export type AlbumPhotos = z.infer<typeof albumPhotosSchema>;
