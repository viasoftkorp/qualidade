interface ILogRocketRequest {
  reqId: string;
  url: string;
  headers: { [key: string]: string | undefined };
  body?: string;
  method: string;
  referrer?: string;
  mode?: string;
  credentials?: string;
}

interface ILogRocketResponse {
  reqId: string;
  status?: number;
  headers: { [key: string]: string | undefined };
  body?: string;
  method: string;
}

// LogRocket IOption Type is not exported, so it's duplicated
export interface ILogRocketOptions {
  release?: string,
  console?: {
    isEnabled?: boolean | {
      log?: boolean,
      info?: boolean,
      debug?: boolean,
      warn?: boolean,
      error?: boolean
    },
    shouldAggregateConsoleErrors?: boolean,
  },
  network?: {
    isEnabled?: boolean,
    requestSanitizer?(request: ILogRocketRequest): null | ILogRocketRequest,
    responseSanitizer?(response: ILogRocketResponse): null | ILogRocketResponse,
  },
  browser?: {
    urlSanitizer?(url: string): null | string,
  },
  dom?: {
    isEnabled?: boolean,
    baseHref?: string,
    textSanitizer?: boolean | string,
    inputSanitizer?: boolean | string,
  },
  /** Controls collection of IP addresses and related features, such as GeoIP **/
  shouldCaptureIP?: boolean,
  /**
    * Enable sharing sessions across subdomains by setting this to the top-level hostname.
    **/
  rootHostname?: string,
  /** Convenience option for configuring the SDK for an on-prem install. Include the protocol (eg. https://ingest.example.com) **/
  ingestServer?: string,
  /** Convenience option for configuring where the full SDK should be loaded from for on-prem installs. **/
  sdkServer?: string,
  uploadTimeInterval?: number,
  /** a callback which determines whether to send data at a particular moment of time. **/
  shouldSendData?(): boolean,
  shouldDebugLog?: boolean,
  mergeIframes?: boolean,
}
