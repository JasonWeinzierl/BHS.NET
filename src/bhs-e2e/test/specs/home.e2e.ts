describe('home', () => {
  it('should display welcome message', async () => {
    await browser.url('/');

    await expect($('app-root h1')).toHaveText('The City Hall Museum sponsored by the Belton Historical Society');
  });

  it('should load banners', async () => {
    await browser.url('/');
    await browser.waitUntil(() => $('alert').isExisting());

    await expect($('alert')).toBeExisting();
  });
});
