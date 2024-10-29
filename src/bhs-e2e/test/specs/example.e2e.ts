describe('example', () => {
    it('should open', async () => {
        await browser.url('https://beltonhistoricalsociety.org/');

        await expect(browser).toHaveTitle('Home | Belton Historical Society');
    });
});
