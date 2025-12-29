import appHeaderPage from '../pageobjects/appHeader.page';
import homePage from '../pageobjects/home.page';
import photosPage from '../pageobjects/photos.page';

describe('photos', () => {
  it('should navigate to Photos', async () => {
    await homePage.open();
    await appHeaderPage.navbarContentMenuButton.click();
    await appHeaderPage.navbarPhotosLink.click();

    await expect(photosPage.title).toHaveText('Photo Albums');
  });

  it('should finish loading without error', async () => {
    await photosPage.open();

    await photosPage.loading.waitForDisplayed({ reverse: true });

    await expect(photosPage.error).not.toBeDisplayed();
    await expect(photosPage.albums).toBeDisplayed();
  });
});
