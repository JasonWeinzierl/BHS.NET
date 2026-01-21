// @ts-check
import { defineConfig } from '@hey-api/openapi-ts';

export default defineConfig({
  input: '../../artifacts/openapi.json',
  output: 'dist',
  plugins: [
    {
      name: 'zod',
      dates: {
        offset: true,
      },
      types: {
        infer: true,
      },
      '~resolvers': {
        // We want actual Date objects when strings are formatted as date-time.
        string(ctx) {
          const { $, schema, symbols } = ctx;
          const { z } = symbols;
          if (schema.format === 'date-time') {
            ctx.nodes.format = () => $(z).attr('coerce').attr('date').call();
          }
          return undefined;
        },
      },
    },
  ],
});
