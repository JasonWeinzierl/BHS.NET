import { SnippetPipe } from './snippet.pipe';

describe('SnippetPipe', () => {
  it('create an instance', () => {
    const pipe = new SnippetPipe();

    expect(pipe).toBeTruthy();
  });

  it('should return input if input is falsy', () => {
    const pipe = new SnippetPipe();
    const input = null;

    const result = pipe.transform(input);

    expect(result).toBe(input);
  });

  it('should return input if input length is less than length', () => {
    const pipe = new SnippetPipe();
    const input = '012';

    const result = pipe.transform(input, 3, 0);

    expect(result).toBe(input);
  });

  it('should truncate if no space is found in fuzz range', () => {
    const pipe = new SnippetPipe();
    const input = '0 23456 89';
    const length = 5;

    const result = pipe.transform(input, length, 1);

    expect(result).toHaveLength(length + 3); // +3 for ellipsis
  });

  it('should go past hard length if space is found in fuzz range', () => {
    const pipe = new SnippetPipe();
    const input = '0 23456 89';

    const result = pipe.transform(input, 5, 3);

    expect(result).toBe('0 23456...');
  });
});
