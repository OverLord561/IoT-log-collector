import { applyMiddleware, combineReducers, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import { routerReducer, routerMiddleware } from 'react-router-redux';
import * as SignUpReducer from '../components/SignUp/logic/signUpReducer';
import * as SignInReducer from '../components/SignIn/logic/signInReducer';
import * as HomeReducer from '../components/Home/logic/homeReducer';
import * as ManageReducer from '../components/Manage/logic/manageReducer';

export default function configureStore(history, initialState) {
  const reducers = {
    signUp: SignUpReducer.signUpReducer,
    signIn: SignInReducer.signInReducer,
    home: HomeReducer.homeReducer,
    manage: ManageReducer.manageReducer
  };

  const middleware = [
    thunk,
    routerMiddleware(history),
  ];

  // In development, use the browser's Redux dev tools extension if installed
  const enhancers = [];
  const isDevelopment = process.env.NODE_ENV === 'development';
  if (isDevelopment && typeof window !== 'undefined' && (window as any).devToolsExtension) {
    //enhancers.push((window as any).devToolsExtension());
  }

  const rootReducer = combineReducers({
    ...reducers,
    routing: routerReducer
  });

  return createStore(
    rootReducer,
    initialState,
    compose(applyMiddleware(...middleware), ...enhancers)
  );
}
