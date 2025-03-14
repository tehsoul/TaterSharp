# API Docs

Best I could find with regards to api 'docs' - from https://starch.one/static/js/lib/api.js

```javascript
let base_url = "https://api.starch.one";

// if (location.host == "localhost") {
//   base_url = "http://localhost:81";
// }

export function get_url() {
  return base_url;
}

function get_request(url) {
  var result = undefined;
  try {
    $.ajax({ url: base_url + url, type: 'GET', dataType: 'json', async: false,
    success: function(data) {result = data;}});
    return result;
  } catch (e) {
    console.error(e);
  } finally {
    return result;
  }
}

function post_request(url, data) {
  var result = undefined;
  try {
    $.ajax({ url: base_url + url, type: 'POST', data: JSON.stringify(data), contentType: "application/json; charset=utf-8", async: false,
      success: function(data) {
        result = data;
      },
      error:function(XMLHttpRequest, textStatus, errorThrown) {
        result = {request: XMLHttpRequest, status: textStatus, errot: errorThrown};
      }
    });
    return result;
  } catch (e) {
    console.error(e);
    return result;
  } finally {
    return result;
  }
}

// general =====================================================================

export function get_status() {
  return get_request("");
}

export function submit_cip8 (signed_data, data) {
  return post_request("/cip8", data);
}

export function get_mint_offers() {
  return get_request("/mint");
}

export function get_mint_timer() {
  return get_request("/mint/timer");
}

export function get_system_time() {
  return get_request("/system_time");
}

// config ======================================================================

export function get_teams_config() {
  return get_request("/config/team");
}

export function get_claim_config() {
  return get_request("/config/claim");
}

export function get_iprc_config() {
  return get_request("/config/iprc");
}

export function get_traits_config() {
  return get_request("/config/traits");
}

// session =====================================================================

export function start_session(stake_addr) {
  return get_request("/session/" + stake_addr + "/start");
}

export function update_session(stake_addr) {
  return get_request("/session/" + stake_addr + "/update");
}

export function session_nfts(stake_addr) {
  return get_request("/session/" + stake_addr + "/nfts");
}

export function session_tokens(stake_addr) {
  return get_request("/session/" + stake_addr + "/tokens");
}

// stats =======================================================================

export function get_starch_holders() {
  return get_request("/market/strch/holders");
}

export function get_starch_holders_graph() {
  return get_request("/market/strch/holders/graph");
}

export function get_starch_prices() {
  return get_request("/market/strch/prices");
}

export function get_starch_prices_graph() {
  return get_request("/market/strch/prices/graph");
}

export function get_tokens_in_app() {
  return get_request("/app/assets/3d77d63dfa6033be98021417e08e3368cc80e67f8d7afa196aaa0b39");
}

// miners ======================================================================

export function get_miner_by_asset_name(asset_name) {
  return get_request("/assets/miner/" + asset_name);
}

export function get_miner_by_id(miner_id) {
  return get_request("/assets/miner/id/" + miner_id);
}

export function get_potato_ids() {
  return get_request("/potatoes");
}

export function get_miner_traits(miner_id) {
  return get_request("/potato/" + miner_id + "/traits");
}

export function redeem_trait(miner_id, code) {
  return post_request("/potato/" + miner_id + "/traits/redeem", {code: code});
}

export function get_miner_traits_inventory(miner_id, layer) {
  return get_request("/potato/" + miner_id + "/traits/" + layer);
}

export function get_miner_achievements(miner_id) {
    return get_request("/achievements/" + miner_id);
}

// market ======================================================================

export function get_starch_circulating_supply_graph() {
  return get_request("/assets/strch/circulating_supply/graph");
}

export function get_starch_circulating_supply() {
  return get_request("/assets/strch/circulating_supply");
}

// miners ======================================================================

export function get_miner_account(miner_id) {
  return get_request("/miners/" + miner_id + "/account")
}

export function get_potato_profile(miner_id) {
  return get_request("/potato/" + miner_id + "/profile");
}

export function get_online_miners_chart() {
  return get_request("/miners/list/online");
}

export function get_online_miners() {
  return get_request("/miners/online");
}

export function get_miner_invites(miner_id) {
  return get_request("/miners/" + miner_id + "/invites");
}

export function get_miner_requests(miner_id) {
  return get_request("/miners/" + miner_id + "/requests");
}

export function get_miner_team(miner_id) {
  return get_request("/miners/" + miner_id + "/team");
}

// teams =======================================================================

export function get_team_account(color_id) {
  return get_request("/teams/" + color_id + "/account");
}

export function get_team_by_color_id(color_id) {
  return get_request("/assets/team/id/" + color_id);
}

export function get_team_by_asset_name(asset_name) {
  return get_request("/assets/team/" + asset_name);
}

export function get_team_config(color_id) {
  return get_request("/teams/" + color_id + "/config");
}

export function get_team_members(color_id) {
  return get_request("/teams/" + color_id + "/members");
}

export function get_team_profile(color_id) {
  return get_request("/teams/" + color_id + "/profile");
}


export function get_team_logs(color_id) {
  return get_request("/teams/" + color_id + "/logs");
}

export function get_team_requests(color_id) {
  return get_request("/teams/" + color_id + "/requests");
}

export function get_team_invites(color_id) {
  return get_request("/teams/" + color_id + "/invites");
}

export function get_teams() {
  return get_request("/teams");
}

export function search_teams(data) {
  return post_request("/teams/search", data);
}

// blockchain ==================================================================

export function get_blockchain_status() {
  return get_request("/blockchain/status");
}

export function get_blocks(page) {
  return get_request("/blockchain/pages/" + page);
}

export function get_block(block_id) {
  return get_request("/blockchain/id/" + block_id);
}

export function get_blocks_by_color(block_id) {
  return get_request("/blockchain/color/" + block_id);
}

export function get_block_vrf(block_id) {
  return get_request("/blockchain/id/" + block_id + "/vrf");
}

export function get_block_timestamp(block_id) {
  return get_request("/blockchain/id/" + block_id + "/timestamp");
}

export function get_block_iprc(block_id) {
  return get_request("/blockchain/id/" + block_id + "/iprc");
}

export function get_block_team(block_id) {
  return get_request("/blockchain/id/" + block_id + "/team");
}

export function get_block_rewards(block_id) {
  return get_request("/blockchain/id/" + block_id + "/rewards");
}

export function get_block_attendance(block_id) {
  return get_request("/blockchain/id/" + block_id + "/attendance");
}

// iprc ========================================================================

export function get_iprc_snapshot(epoch) {
  return get_request("/iprc/" + epoch);
}

export function get_iprc_list() {
  return get_request("/iprc/list");
}

export function get_iprc_fund(epoch) {
  return get_request("/iprc/" + epoch + "/fund");
}

export function get_pool(ticker) {
  return get_request("/pools/" + ticker);
}

export function get_pool_delegation(ticker) {
  return get_request("/pools/" + ticker + "/delegation");
}

export function get_miner_pool_ticker(miner_id) {
  return get_request("/iprc/miner_id/" + miner_id);
}

export function get_miner_pool_snapshot(epoch) {
  return get_request("/iprc/epoch/" + epoch);
}

// signatures ==================================================================

export function get_signature_data(signature_id) {
  return get_request("/cip8/id/" + signature_id);
}

export function get_signature_raw(signature_id) {
  return get_request("/cip8/id/" + signature_id + "/raw");
}

export function get_signature_owner(signature_id) {
  return get_request("/cip8/id/" + signature_id + "/owner");
}

export function get_last_hash() {
  return get_request("/blockchain/last_hash");
}

export function get_last_timestamp() {
  return get_request("/blockchain/last_timestamp");
}

export function get_pending_blocks() {
  return get_request("/pending_blocks");
}

export function get_pending_block(miner_id) {
  return get_request("/pending_blocks/" + miner_id);
}

export function submit_block(block) {
  return post_request("/submit_block", block);
}
export function submit_blocks(blocks) {
  return post_request("/submit_blocks", blocks);
}

// news ========================================================================

export function get_news() {
  return get_request("/news");
}

export function get_news_page() {
  return get_request("/news/0");
}

// leaderboard =================================================================

export function get_weekly_leaderboard() {
  return get_request("/leaderboard/week");
}

export function get_miner_weekly_leaderboard(miner_id) {
  return get_request("/leaderboard/miners/" + miner_id + "/week");
}

export function get_miner_leaderboard_wins(miner_id) {
  return get_request("/leaderboard/miners/" + miner_id + "/wins");
}

export function get_leaderboard_winners() {
  return get_request("/leaderboard/winners");
}

export function get_leaderboard_ranks() {
  return get_request("/leaderboard/ranks");
}

export function get_leaderboard_miner_rank(miner_id) {
  return get_request("/leaderboard/ranks/" + miner_id);
}

// =============================================================================

export function get_miner_attendance_list(miner_id) {
  return get_request("/miners/" + miner_id + "/attendance");
}

export function get_miner_blocks(miner_id) {
  return get_request("/miners/" + miner_id + "/blocks");
}

// addresses ===================================================================

export function get_stake_info(stake_addr) {
  return get_request("/addresses/" + stake_addr + "/stake");
}
```