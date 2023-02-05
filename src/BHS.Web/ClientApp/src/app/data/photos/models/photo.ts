export interface Photo {
  id: string;
  legacyId: number;
  name?: string;
  imagePath: string;
  datePosted: Date;
  authorUsername?: string;
  description?: string;
}
