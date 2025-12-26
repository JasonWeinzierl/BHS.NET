import homePage from '../pageobjects/home.page';

describe('home', () => {
  it('should display welcome message', async () => {
    await homePage.open();

    await expect(homePage.self).toBeDisplayed();
    await expect(homePage.title).toHaveText('The City Hall Museum sponsored by the Belton Historical Society');
  });
});
