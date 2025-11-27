export interface PlanoAmostragem {
    id: string;
    quantidadeMinima: number;
    quantidadeMaxima: number;
    quantidadeInspecionar: number;
}

export interface GetAllPlanoAmostragem {
    items: PlanoAmostragem[];
    totalCount: number;
}