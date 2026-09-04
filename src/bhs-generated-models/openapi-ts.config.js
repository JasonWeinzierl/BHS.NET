// @ts-check
import { defineConfig } from '@hey-api/openapi-ts';
import path from 'node:path';

const __dirname = import.meta.dirname;

export default defineConfig({
  interactive: false,
  input: path.join(__dirname, '../../artifacts/openapi.json'),
  output: path.join(__dirname, 'dist'),
  plugins: [
    {
      name: 'zod',
      dates: {
        offset: true,
      },
      types: {
        infer: true,
      },
      $resolvers: {
        // We want actual Date objects when strings are formatted as date-time.
        string(ctx) {
          const { $, schema, symbols } = ctx;
          const { z } = symbols;
          if (schema.format === 'date-time') {
            ctx.nodes.format = () => $(z).attr('coerce').attr('date').call();
          }
          return undefined;
        },
        object(ctx) {
          const { $, schema, symbols } = ctx;
          const { z } = symbols;

          const shape = ctx.nodes.shape(ctx);
          for (const [key, propSchema] of Object.entries(schema.properties || {})) {
            if (!propSchema.items) continue;
            // System.Text.Json's number handling causes the spec to generate union[string, integer] for integers.
            // Change it back to integer.
            if (propSchema.format === 'int32') {
              const itemTypes = new Set(propSchema.items.map(item => item.type));
              if (
                itemTypes.size === 2
                && itemTypes.has('integer')
                && itemTypes.has('string')
              ) {
                shape.prop(key, $(z).attr('int').call());
              } else if (
                itemTypes.size === 3
                && itemTypes.has('integer')
                && itemTypes.has('string')
                && itemTypes.has('null')
              ) {
                shape.prop(key, $(z).attr('int').call().attr('nullish').call());
              }
              continue;
            }

          }

          return $(z).attr('object').call(shape);
        },
      },
    },
  ],
});
