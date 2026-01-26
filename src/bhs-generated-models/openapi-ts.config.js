// @ts-check
import { defineConfig } from '@hey-api/openapi-ts';

export default defineConfig({
  interactive: false,
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
        object(ctx) {
          const { $, schema, symbols } = ctx;
          const { z } = symbols;
          
          const shape = ctx.nodes.shape(ctx);
          for (const [key, propSchema] of Object.entries(schema.properties || {})) {
            if (!propSchema.items) continue;
            // System.Text.Json's number handling causes the spec to generate union[string, integer] for integers.
            // Change it back to integer.
            if (propSchema.format === 'int32') {
              if (
                propSchema.items.length === 2
                && propSchema.items.some(x => x.type === 'integer')
                && propSchema.items.some(x => x.type === 'string')
              ) {
                shape.prop(key, $(z).attr('int').call());
              } else if (
                propSchema.items.length === 3
                && propSchema.items.some(x => x.type === 'integer')
                && propSchema.items.some(x => x.type === 'string')
                && propSchema.items.some(x => x.type === 'null')
              ) {
                shape.prop(key, $(z).attr('int').call().attr('nullish').call());
              }
              continue;
            }

            // Convenience change nullable primitives to nullish.
            if (
              propSchema.items.length === 2
              && propSchema.items.some(x => x.type === 'null')
            ) {
              if (
                propSchema.items.some(x => x.type === 'string' && !x.format)
              ) {
                shape.prop(key, $(z).attr('string').call().attr('nullish').call());
              } else if (
                propSchema.items.some(x => x.type === 'integer')
              ) {
                shape.prop(key, $(z).attr('int').call().attr('nullish').call());
              } else if (
                propSchema.items.some(x => x.type === 'boolean')
              ) {
                shape.prop(key, $(z).attr('boolean').call().attr('nullish').call());
              } else if (
                propSchema.items.some(x => x.type === 'number')
              ) {
                shape.prop(key, $(z).attr('number').call().attr('nullish').call());
              } else if (
                propSchema.items.some(x => x.type === 'string' && x.format === 'date-time')
              ) {
                shape.prop(key, $(z).attr('coerce').attr('date').call().attr('nullish').call());
              } else if (
                propSchema.items.some(x => x.type === 'string' && x.format === 'uri')
              ) {
                shape.prop(key, $(z).attr('url').call().attr('nullish').call());
              }
            }

          }

          return $(z).attr('object').call(shape);
        }
      },
    },
  ],
});
