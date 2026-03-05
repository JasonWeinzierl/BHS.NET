import Page from './page';

export class PostCardPage extends Page {
  constructor(public readonly slug: string) {
    super();
  }

  get self() {
    return this.getByTestID(`PostCard-${this.slug}`);
  }

  get title() {
    return this.getByTestID(`PostCard-${this.slug}-Title`);
  }

  get postedInfo() {
    return this.getByTestID(`PostCard-${this.slug}-Posted`);
  }

  get contentPreview() {
    return this.getByTestID(`PostCard-${this.slug}-ContentPreview`);
  }

  get readFullPostButton() {
    return this.getByTestID(`PostCard-${this.slug}-ReadFullPostButton`);
  }

  get categories() {
    return this.getByTestID(`PostCard-${this.slug}-Categories`);
  }
}
