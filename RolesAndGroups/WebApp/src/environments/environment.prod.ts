import {Environment, getCurrentHostAddress} from './environment.common';

export const environment: Environment = {
  production: true,
  api: {
    get clientApi(): string {
      return getCurrentHostAddress(4430);
    }
  }
};
