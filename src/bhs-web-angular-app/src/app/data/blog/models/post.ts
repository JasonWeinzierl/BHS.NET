import { PostZodType, zPost } from 'bhs-generated-models';

export const postSchema = zPost;

export type Post = PostZodType;
