describe('photos', () => {
  it('should navigate to Photos', async () => {
    await browser.url('/');
    await $('a=Content').click();
    await $('a=Photos').click();
    await expect($('h1')).toHaveText('Photo Albums');
  });
});
