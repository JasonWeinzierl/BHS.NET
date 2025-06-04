describe('photos', () => {
  it('should navigate to Photos', async () => {
    await browser.url('/');
    await $('button=Content').click();
    await $('a=Photos').click();
    await expect($('h1')).toHaveText('Photo Albums');
  });
});
