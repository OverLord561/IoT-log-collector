import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import SignUp from './components/SignUp/SignUp';
import SignIn from './components/SignIn/SignIn';

export default () => (
  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/sign-up' component={SignUp} />
    <Route path='/sign-in' component={SignIn} />

    {/* <Route path='/fetchdata/:startDateIndex?' component={FetchData} /> */}
  </Layout>
);
