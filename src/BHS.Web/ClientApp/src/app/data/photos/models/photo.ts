export interface Photo {
  id: string;
  name?: string;
  imagePath: string;
  datePosted: Date;
  authorUsername?: string;
  description?: string;
}
