## Table of Contents

Change config values to suit your server economy. NOTE: RENAMING IMMINENT

- [Features](#features)
- [Commands](#commands)
- [Configuration](#configuration)

## Features

- **Nocturnes:** Reward players with Nocturnes for being online. Can be redeemed in-game for a configurable item reward, Nocturnes per minute online and Nocturnes per item reward can be configured as well as the update interval.
- **Daily Login Rewards:** Reward players with a congfigurable item/quantity for logging in once per day. Can be separate from Nocturnes item.

## Commands

### Nocturne Commands
- `.redeemNocturnes`
  - Redeem Nocturnes for items.
  - Shortcut: *.rn*
- `.getNocturnes`
  - Get total Nocturnes. Also updates them.
  - Shortcut: *.get n*
 
## Configuration

### Reward Systems
- **Nocturnes**: `Nocturnes` (bool, default: false)  
  Enable or disable Nocturnes.
- **Daily Logins**: `DailyLogin` (bool, default: false)  
  Enable or disable daily logins. Nocturnes must be enabled as well.
- **Daily Item Reward**: `DailyReward` (int, default: -257494203)  
  Item prefab for daily reward.
- **Daily Item Quantity**: `DailyQuantity` (int, default: 50)
  Item quantity for daily reward.
- **Nocturnes Item Reward**: `NocturnesItemReward` (int, default: -257494203)  
  Item prefab for redeeming Nocturnes.
- **Nocturnes Reward Factor**: `NocturnesRewardFactor` (int, default: 50)  
  Nocturnes required per item reward.
- **Nocturnes Per Minute**: `NocturnesPerMinute` (int, default: 5)  
  Factor by which rates are increased in expertise/legacy per increment of prestige in leveling.
- **Nocturnes Update Interval**: `NocturnesUpdateInterval` (int, default: 30)  
  Interval in minutes to update player Nocturnes.

