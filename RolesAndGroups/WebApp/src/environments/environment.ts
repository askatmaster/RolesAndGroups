import {Environment, getCurrentHostAddress} from './environment.common';

export const environment: Environment = {
  production: false,
  api: {
    get clientApi(): string {
      return getCurrentHostAddress(7073);
    }
  }
};

