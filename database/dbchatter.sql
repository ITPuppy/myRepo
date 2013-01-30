/*
Navicat MySQL Data Transfer

Source Server         : Fighter
Source Server Version : 50529
Source Host           : localhost:3306
Source Database       : dbchatter

Target Server Type    : MYSQL
Target Server Version : 50529
File Encoding         : 65001

Date: 2013-01-29 15:43:17
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `tblfriend`
-- ----------------------------
DROP TABLE IF EXISTS `tblfriend`;
CREATE TABLE `tblfriend` (
  `id` varchar(15) NOT NULL,
  `groupId` varchar(10) NOT NULL DEFAULT '',
  `groupName` varchar(100) DEFAULT NULL,
  `friendId` mediumtext NOT NULL,
  PRIMARY KEY (`id`,`groupId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tblfriend
-- ----------------------------
INSERT INTO `tblfriend` VALUES ('4177319', '0', '我的好友', '776041668;');
INSERT INTO `tblfriend` VALUES ('4177319', '97', '我的家人', '776041669;');
INSERT INTO `tblfriend` VALUES ('776041668', '0', '我的好友', '776041669;4177319;');
INSERT INTO `tblfriend` VALUES ('776041669', '0', '我的好友', '776041668;4177319;');

-- ----------------------------
-- Table structure for `tblgroup`
-- ----------------------------
DROP TABLE IF EXISTS `tblgroup`;
CREATE TABLE `tblgroup` (
  `groupId` varchar(15) NOT NULL,
  `ownerId` varchar(15) NOT NULL,
  `groupMember` mediumtext NOT NULL,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`groupId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tblgroup
-- ----------------------------

-- ----------------------------
-- Table structure for `tblmember`
-- ----------------------------
DROP TABLE IF EXISTS `tblmember`;
CREATE TABLE `tblmember` (
  `id` varchar(15) NOT NULL,
  `nickName` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `sex` char(10) NOT NULL,
  `birthday` date NOT NULL,
  `information` mediumtext NOT NULL,
  `status` varchar(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tblmember
-- ----------------------------
INSERT INTO `tblmember` VALUES ('4177319', 'Tom', 'vbnvbn', '男', '1981-03-03', '', '0');
INSERT INTO `tblmember` VALUES ('776041668', 'eva', 'vbnvbn', '男', '1981-03-03', '', '');
INSERT INTO `tblmember` VALUES ('776041669', 'walle', 'vbnvbn', '男', '1981-03-03', '', '');
