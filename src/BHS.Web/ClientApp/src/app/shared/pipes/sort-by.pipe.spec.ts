import { SortByPipe } from './sort-by.pipe';

describe('SortByPipe', () => {
  it('create an instance', () => {
    const pipe = new SortByPipe();

    expect(pipe).toBeTruthy();
  });

  it('should return null if the input is null', () => {
    const pipe = new SortByPipe();

    const result = pipe.transform(null);

    expect(result).toBeNull();
  });

  it('should return the input if value length is 1', () => {
    const pipe = new SortByPipe();
    const input = [1];

    const result = pipe.transform(input);

    expect(result).toBe(input);
  });

  it('should use the default sort if no column is provided', () => {
    const pipe = new SortByPipe();
    const input = ['b', 'a', 'c'];

    const result = pipe.transform(input);

    expect(result).toEqual(['a', 'b', 'c']);
  });

  it('should use reverse default sort', () => {
    const pipe = new SortByPipe();
    const input = ['b', 'a', 'c'];

    const result = pipe.transform(input, 'desc');

    expect(result).toEqual(['c', 'b', 'a']);
  });

  it('should sort by column', () => {
    const pipe = new SortByPipe();
    const input = [{ foo: 'b' }, { foo: 'a' }, { foo: 'c' }];

    const result = pipe.transform(input, 'asc', 'foo');

    expect(result).toEqual([{ foo: 'a' }, { foo: 'b' }, { foo: 'c' }]);
  });

  it('should sort by column in reverse', () => {
    const pipe = new SortByPipe();
    const input = [{ foo: 'b' }, { foo: 'a' }, { foo: 'c' }];

    const result = pipe.transform(input, 'desc', 'foo');

    expect(result).toEqual([{ foo: 'c' }, { foo: 'b' }, { foo: 'a' }]);
  });
});
