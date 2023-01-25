export interface Environment {
  production: boolean;
  api: {
    clientApi: string;
  };
}

export function getCurrentHostAddress(port: number): string {
  return `https://${window.location.hostname}:${port}`;
}
