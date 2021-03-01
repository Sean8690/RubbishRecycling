import * as ReactDOM from 'react-dom';
import { App } from './App';
import * as React from 'react';
import { Zenith } from '@infotrack/zenith-ui';

ReactDOM.render(
  <Zenith useToastContext={false}>
    <App />
  </Zenith>,
  document.getElementById('root')
);
