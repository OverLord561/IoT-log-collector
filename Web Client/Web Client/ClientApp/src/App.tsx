import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home/Home';
import SignUp from './components/SignUp/SignUp';
import SignIn from './components/SignIn/SignIn';
import Manage from './components/Manage/Manage';

export default class App extends React.Component<any, any> {
  public render() {
    return <Layout>
      <Route exact path='/' component={Home} />
      <Route path='/sign-up' component={SignUp} />
      <Route path='/sign-in' component={SignIn} />
      <Route path='/manage' component={Manage} />

      {/* <Route path='/fetchdata/:startDateIndex?' component={FetchData} /> */}
    </Layout>;
  }
}
