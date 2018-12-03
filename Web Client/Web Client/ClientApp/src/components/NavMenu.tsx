import React from 'react';
import { Link } from 'react-router-dom';
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';
import { IApplicationState } from '../store/index';
import { connect } from 'react-redux';

interface IStateToProps {
  authorized: boolean;
}

type IProps = IStateToProps;

export class NavMenu extends React.Component<IProps, any> {
  constructor(props: IProps) {
    super(props);
  }

  public render() {
    return <Navbar inverse fixedTop fluid collapseOnSelect>
      <Navbar.Header>
        <Navbar.Brand>
          <Link to={'/'}>IoT Web Client</Link>
        </Navbar.Brand>
        <Navbar.Toggle />
      </Navbar.Header>
      <Navbar.Collapse>
        <Nav>
          <LinkContainer to={'/'} exact>
            <NavItem>
              <Glyphicon glyph='home' /> Home
          </NavItem>
          </LinkContainer>
          <LinkContainer to={'/sign-up'}>
            <NavItem>
              <Glyphicon glyph='registration-mark' /> Sign Up
            </NavItem>
          </LinkContainer>
          {!this.props.authorized &&
            <LinkContainer to={'/sign-in'}>
              <NavItem>
                <Glyphicon glyph='log-in' /> Sign In
              </NavItem>
            </LinkContainer>
          }

          {this.props.authorized &&
            <LinkContainer to={'/log-out'}>
              <NavItem>
                <Glyphicon glyph='log-out' /> Log Out
              </NavItem>
            </LinkContainer>
          }

        </Nav>
      </Navbar.Collapse>
    </Navbar>;
  }
}

const mapStateToProps = (state: IApplicationState): IStateToProps => {
  return {
    authorized: state.signIn.authorized,
  };
};

export default connect(
  mapStateToProps, // Selects which state properties are merged into the component's props
  {} // Selects which action creators are merged into the component's props
)(NavMenu);
