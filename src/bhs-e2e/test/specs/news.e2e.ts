import { PostPreviewZodType, zPostPreview } from 'bhs-generated-models';
import { Agent, fetch } from 'undici';
import appHeaderPage from '../pageobjects/appHeader.page';
import blogPage from '../pageobjects/blog.page';
import homePage from '../pageobjects/home.page';

describe('news', () => {
  let firstPost: PostPreviewZodType;
  let postsCount: number;

  beforeAll(async () => {
    if (!browser.options.baseUrl) {
      throw new Error('baseUrl is not set.');
    }

    const response = await fetch(browser.options.baseUrl + '/api/blog/posts', {
      dispatcher: new Agent({
        connect: {
          rejectUnauthorized: false,
        },
      }),
    });

    expect(response.status).toBe(200);

    const posts = zPostPreview.array().parse(await response.json());
    if (!posts.length) {
      throw new Error('No posts found. Cannot test UI without sample data.');
    }
    posts.sort((a, b) => b.datePublished.getTime() - a.datePublished.getTime());

    firstPost = posts[0];
    postsCount = posts.length;
  });

  it('should navigate to News', async () => {
    await homePage.open();

    await appHeaderPage.navbarNewsLink.click();

    await expect(blogPage.title).toHaveText('News');
  });

  it('should first post', async () => {
    await blogPage.open();

    const firstPostPage = await blogPage.getFirstPost();

    await expect(firstPostPage.self).toBeDisplayed();

    await expect(firstPostPage.title).toHaveText(firstPost.title);
    await expect(firstPostPage.contentPreview).toHaveText(expect.stringContaining(firstPost.contentPreview.replace(/\n\n*/g, ' ')));
    await expect(firstPostPage.postedInfo).toHaveText(expect.stringContaining(firstPost.datePublished.getFullYear().toString()));
    if (firstPost.author?.name) {
      await expect(firstPostPage.postedInfo).toHaveText(expect.stringContaining(firstPost.author.name));
    }
  });

  xit('should list all posts', async () => {
    await blogPage.open();

    expect(await $$('app-post-card').length).toBe(postsCount);
  });
});
