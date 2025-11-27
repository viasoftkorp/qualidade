const ids : string[] = [
  "6B1FA7AB-D290-4A75-96BD-7E561AA7BA84",
  "B89150D8-9603-481D-AAC4-1C26C4544F56",
  "7757C843-FB73-4651-9D9F-7E3996301601",
  "9DDC4F92-C0A5-4F53-B4A7-33CDF62C5958",
  "089F1738-1B63-461D-8B5B-63C6D7AD16D6",
  "4303A98B-7C5B-4782-9A52-2825A0905227",
  "303AEC23-F80C-41C6-BD4A-7BB526BD5D11",
  "03710A52-A9AB-4933-8BF3-A6BE121B4CE7",
  "41B01E15-33AA-429B-9F7A-7FC99E1B03E8",
  "C666B290-D3D1-4DFF-8108-BFC61E4E973C",
  "C541ACA4-2B6F-44F6-A991-3B441B47E3E8",
  "B03C4009-4E80-4236-A3AB-A1CFAA6FFAB6",
  "D6BF1ECF-DE7F-43B2-8A1C-546C1E8E8EC1",
  "05D3C03B-4EEF-481B-84CF-2F5AD4B527FB",
];

const strings: string[] = [
  "Encefalite",
  "Tuberculose",
  "Asma",
  "Sifilis",
  "Cancer",
  "COVID-19",
  "Gripe",
  "Meningite",
  "Laringite",
  "Orquite",
  'Fimose'
];
const codes: number[] = [
  1,
  2,
  3,
  4,
  5,
  6,
  7,
  8,
  9,
  10

]
const dates: Date[] = [
  new Date(2022, 0, 23),
  new Date(2022, 1, 23),
  new Date(2022, 2, 23),
  new Date(2022, 3, 23),
  new Date(2022, 4, 23),
  new Date(2022, 5, 23),
  new Date(2022, 6, 23),
  new Date(2022, 7, 23),
  new Date(2022, 8, 23),
  new Date(2022, 9, 23),
  new Date(2022, 10, 23),
  new Date(2022, 11, 23),
];
const validEmails: Array<string> = [
  'encefalite@doença.com',
  'gonorreia@ferrou.com',
  'covid@jaPassou.com'
]
export function getRequestsFromMock(mock: any): CypressRequestV2<any>[] {
  return Object.values(mock).filter(property => property !== undefined).map(property => property as CypressRequestV2<any>);
}

export {strings,ids,codes,dates, validEmails}






