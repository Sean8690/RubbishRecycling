import { IPersonListResponseDto } from '../api';

export interface DowJonesState {
  lookupResults: IPersonListResponseDto;
  searchedName: string;
}
