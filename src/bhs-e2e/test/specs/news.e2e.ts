import { Agent, fetch } from 'undici';
import { z } from 'zod';
import appHeaderPage from '../pageobjects/appHeader.page';
import blogPage from '../pageobjects/blog.page';
import homePage from '../pageobjects/home.page';

// TODO: dedupe with web app, define models in a separate package.
const postPreviewSchema = z.object({
  slug: z.string(),
  title: z.string(),
  contentPreview: z.string(),
  author: z.object({
    username: z.string(),
    name: z.string().nullish(),
  }).nullish(),
  datePublished: z.coerce.date(),
});
type PostPreview = z.infer<typeof postPreviewSchema>;

describe('news', () => {
  let firstPost: PostPreview;
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

    const posts = postPreviewSchema.array().parse(await response.json());
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

  it('should list all posts', async () => {
    await blogPage.open();

    const posts = await blogPage.getPostsList();

    expect(posts.length).toBe(postsCount);
  });
});
