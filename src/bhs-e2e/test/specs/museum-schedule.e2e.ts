import appHeaderPage from '../pageobjects/appHeader.page';
import homePage from '../pageobjects/home.page';
import locationPage from '../pageobjects/location.page';

describe('museum-schedule', () => {
  beforeAll(async () => {
    await homePage.open();
  });

  it('should navigate to location from home', async () => {
    await appHeaderPage.navbarContentMenuButton.click();
    await appHeaderPage.navbarLocationLink.click();

    await expect(locationPage.self).toBeDisplayed();
  });

  it('should stop loading without error', async () => {
    await expect(locationPage.scheduleLoading).toBeDisplayed();

    await locationPage.scheduleLoading.waitForDisplayed({ reverse: true, timeout: 10000 });

    await expect(locationPage.scheduleError).not.toBeDisplayed();
  });

  it('should display the schedule', async () => {
    await expect(locationPage.scheduleContainer).toBeDisplayed();
    await expect(locationPage.scheduleContainer).toHaveText(expect.stringContaining('from'));
  });
});
