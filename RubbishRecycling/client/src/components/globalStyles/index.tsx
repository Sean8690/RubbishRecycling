import { Global } from '@emotion/core';
import * as React from 'react';

export function GlobalStyles() {
  return (
    <Global
      styles={() => ({
        '#itz-layout-main': {
          padding: '0 0 1.5rem 0 !important'
        },
        'html *': {
          fontFamily: "'Inter', sans-serif"
        }
      })}
    />
  );
}
