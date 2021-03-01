import React, { ReactNode, useContext, useState } from 'react';
import { DowJonesState } from '../models';

const defaultDowJonesState: DowJonesState = {
  lookupResults: { clientReference: '' },
  searchedName: ''
};

export const DowJonesContext = React.createContext<
  [DowJonesState, (state: DowJonesState) => void]
>([defaultDowJonesState, () => null]);

interface GlobalStateProviderProps {
  children: ReactNode;
}

export function DowJonesContextProvider(props: GlobalStateProviderProps) {
  const { children } = props;

  const [globalState, setGlobalState] = useState({ ...defaultDowJonesState });

  const updateGlobalState = (state: DowJonesState) => {
    const newState = { ...state };
    setGlobalState(newState);
  };

  return (
    <DowJonesContext.Provider value={[globalState, updateGlobalState]}>
      {children}
    </DowJonesContext.Provider>
  );
}

/** Provides a global "useDowJonesContext", returning an array with 2 elements,
 *  the fist one is the current global state object, the second element
 *  is a function to update it once it has changed in your component logic.
 */
export const useDowJonesContext = () => useContext(DowJonesContext);
