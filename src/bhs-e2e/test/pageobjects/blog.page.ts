import Page from './page';
import { PostCardPage } from './post-card.page';

class BlogPage extends Page {
  override open() {
    return super.open('/apps/blog');
  }

  get self() {
    return this.getByTestID('BlogIndex');
  }

  get title() {
    return this.getByTestID('BlogIndex-Title');
  }

  get newPostButton() {
    return this.getByTestID('BlogIndex-NewPostButton');
  }

  async getFirstPost(): Promise<PostCardPage> {
    const firstPostTestId = await $('app-post-card')
      .getAttribute('data-testid');

    if (!firstPostTestId) {
      throw new Error('No post cards found on the blog index page.');
    }

    const postSlug = firstPostTestId
      .replace('PostCard-', '');

    return new PostCardPage(postSlug);
  }
}
export default new BlogPage();
