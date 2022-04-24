export interface Photo {
  id: number;
  name?: string;
  imagePath: string;
  datePosted: Date;
  authorId?: number;
  description?: string;
}
