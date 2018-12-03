export interface IModel {
    isFetching: boolean;
    errors: IValidationError[];
}

export interface IValidationError {
    exception: string;
    errorMessage: string;
}
